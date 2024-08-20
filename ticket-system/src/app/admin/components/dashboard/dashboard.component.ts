import { TicketsByPriority } from './../../../core/model/dashboard-models/ticketsByPriority';
import { Component, OnInit } from '@angular/core';
import * as echarts from 'echarts';
import { DashboardService } from '../../../shared/services/dashboard.service';
import { APIResponse } from '../../../core/model/APIResponse';
import { Status } from '../../../core/model/enums/Status';
import { Priorities } from '../../../core/model/enums/Priorities';
import { TopEmployee } from '../../../core/model/TopEmployees';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Stat } from '../../../core/model/statistics';
import { Observable } from 'rxjs';
import { TranslateModule } from '@ngx-translate/core';
import { TicketsByStatus } from '../../../core/model/dashboard-models/ticketsByStatus';
import { MatIconModule } from '@angular/material/icon';
import { TicketsByMonthsOfYear } from '../../../core/model/dashboard-models/ticektsBymonths';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslateModule, MatIconModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'], // Corrected from "styleUrl"
})
export class DashboardComponent implements OnInit {
  ticketPerYear2024: any;
  ticketPerMonth8_2024: any;

  ticketsOfYearMyMonths: TicketsByMonthsOfYear[] = [];
  totalTicketsSummary: any;
  totalTickets: any;
  openTicketsWithHighPriority: any;
  topEmployees!: TopEmployee[];
  stat!: Stat;
  ticketsByStatus!: TicketsByStatus;

  chartInstance: any;
  chartInstance2: any;
  chartInstance3: any;
  chartInstance4: any;
  ticketsByPriority!: TicketsByPriority;

  constructor(private dashService: DashboardService) {}

  ngOnInit(): void {
    this.loadTicketsForTheCurrentMonth();
    this.loadTicketsPerCurrentYear();
    //  this.getOpenTicketsWithHighPriority();
    this.getTopEmployees();
    this.loadStat();
    this.loadAllTicketsWithStatus();
    this.getTicketsOfYearByMonths();
    this.getTicketsGroupByPriority();
  }

  getTicketsGroupByPriority() {
    this.dashService
      .getTicketGroupByPrioruty()
      .subscribe((res: APIResponse) => {
        console.log(res);

        this.ticketsByPriority = Object(res.data);
        console.log(res.data);
        this.initChatTicketsByPriority();
      });
  }
  getTicketsOfYearByMonths() {
    this.dashService
      .ticketsOfTheYearGroupByMonths()
      .subscribe((res: APIResponse) => {
        if (res.status) {
          //    console.log(res.data);
          this.ticketsOfYearMyMonths = res.data;
          this.initChartOfTicketsOfYear();
        }
      });
  }

  loadStat() {
    this.dashService.getStat().subscribe((res: APIResponse) => {
      this.stat = Object(res.data);
    });
  }
  loadAllTicketsWithStatus() {
    this.dashService.getTicketsByStatus().subscribe((res: APIResponse) => {
      if (res.status) {
        this.ticketsByStatus = Object(res.data);
        // console.log(this.ticketsByStatus);
        this.initChart();
      }
    });
  }

  loadTicketsForTheCurrentMonth() {
    this.dashService
      .getTicketsPerMonthManger(2024, 8)
      .subscribe((res: APIResponse) => {
        this.ticketPerMonth8_2024 = res.data[0].ticketsCount;
      });
  }

  getTotalTickets() {
    // Implement this method if needed
  }

  getTopEmployees() {
    this.dashService.getTopEmployeesManger().subscribe((res: APIResponse) => {
      this.topEmployees = res.data;
    });
  }

  // getOpenTicketsWithHighPriority() {
  //   this.dashService
  //     .getTotalTicketsSummary(Status.InProgress, Priorities.High)
  //     .subscribe((res) => {
  //       this.openTicketsWithHighPriority = res.data;
  //     });
  // }

  loadTicketsPerCurrentYear() {
    this.dashService
      .getTicketsPerYearManger(2024)
      .subscribe((res: APIResponse) => {
        this.ticketPerYear2024 = res.data[0].ticketsCount;
      });
  }

  initChartOfTicketsOfYear() {
    const x = this.ticketsOfYearMyMonths.reverse();

    const chartDom = document.getElementById('mainChartOfTicketByYear')!;
    const date = new Date().getMonth() + 1;
    //  console.log(date);
    const obj = x.slice(0, date);
    const months = [
      'Jan',
      'Feb',
      'Mar',
      'Apr',
      'May',
      'Jun',
      'Jul',
      'Aug',
      'Sep',
      'Oct',
      'Nov',
      'Dec',
    ];
    const data: number[] = [];
    obj.forEach((item) => data.push(item.ticketCount));

    // console.log(data);
    const monthSliced = months.slice(0, date);

    this.chartInstance4 = echarts.init(chartDom);
    this.chartInstance4.setOption({
      xAxis: {
        type: 'category',
        boundaryGap: false,
        data: monthSliced,
      },
      yAxis: {
        type: 'value',
      },
      series: [
        {
          data: data,

          type: 'line',
          areaStyle: {},
        },
      ],
    });
  }
  initChatTicketsByPriority() {
    const chartDom = document.getElementById('chartOfTicketByPriority')!;
    this.chartInstance3 = echarts.init(chartDom);

    this.chartInstance3.setOption({
      tooltip: {
        trigger: 'item',
      },
      legend: {
        top: '5%',
        left: 'center',
      },
      series: [
        {
          name: 'Access From',
          type: 'pie',
          radius: ['40%', '70%'],
          center: ['50%', '70%'],
          // adjust the start and end angle
          startAngle: 180,
          endAngle: 360,
          data: [
            { value: this.ticketsByPriority.low, name: 'Low' },
            { value: this.ticketsByPriority.medium, name: 'Medium' },
            { value: this.ticketsByPriority.high, name: 'High' },
          ],
        },
      ],
    });
  }
  initChart() {
    const chartDom = document.getElementById('mainChart')!;
    this.chartInstance2 = echarts.init(chartDom);
    this.chartInstance2.setOption({
      tooltip: {
        trigger: 'item',
      },

      legend: {
        top: '10%',
        left: 'center',
      },
      series: [
        {
          top: '15%',
          name: 'Ticket Status',
          type: 'pie',
          radius: ['40%', '80%'],
          avoidLabelOverlap: false,
          itemStyle: {
            borderRadius: 10,
            borderColor: '#fff',
            borderWidth: 2,
          },
          label: {
            show: false,
            position: 'center',
          },
          emphasis: {
            label: {
              show: true,
              fontSize: 40,
              fontWeight: 'bold',
            },
          },
          labelLine: {
            show: false,
          },
          data: [
            { value: this.ticketsByStatus.new, name: 'New' },
            { value: this.ticketsByStatus.inProgress, name: 'In progress' },
            { value: this.ticketsByStatus.resolved, name: 'Resolved' },
            { value: this.ticketsByStatus.closed, name: 'Closed' },
            { value: this.ticketsByStatus.reOpened, name: 'Re-Opened' },
          ],
        },
      ],
    });
  }
}
