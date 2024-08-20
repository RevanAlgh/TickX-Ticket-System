import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { AuthService } from '../../../core/services/auth.service';
import { DashboardService } from '../../../shared/services/dashboard.service';
import { APIResponse } from '../../../core/model/APIResponse';
import { TicketsByMonthsOfYear } from '../../../core/model/dashboard-models/ticektsBymonths';
import { Priorities } from '../../../core/model/enums/Priorities';
import * as echarts from 'echarts';
import { TicketsByStatus } from '../../../core/model/dashboard-models/ticketsByStatus';
import { TopEmployee } from '../../../core/model/TopEmployees';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslateModule],
  templateUrl: './dashboard.component.html',
  styleUrl: '../../../admin/components/dashboard/dashboard.component.css',
})
export class DashboardComponent implements OnInit {
  ticketPerMonth!: { ticketsCount: 0; month: 0 };
  stat: any;

  chartInstance2: any;
  chartInstance3: any;

  ticketsOfYear!: number;
  ticketsByPriority: any;
  ticketsByStatus!: TicketsByStatus;
  topEmployees!: TopEmployee[];

  constructor(
    private auth: AuthService,
    private dashService: DashboardService
  ) {}
  ngOnInit(): void {
    this.loadTicketsOfMonth();
    this.getTicketsGroupByStat();
    this.getTicketsGroupByPriority();
    this.loadTicketsPerYear();
    this.getTopEmployees();
  }

  loadTicketsOfMonth() {
    const month = new Date().getMonth() + 1;
    const year = new Date().getFullYear();

    console.log(month, year);
    this.dashService
      .getTicketsPerMonthUser(this.auth.getUserID(), month, year)
      .subscribe((res: APIResponse) => {
        this.ticketPerMonth = res.data[0];
        this.stat = res.data;
        ///    console.log(this.ticketPerMonth.ticketsCount);

        if (res.status) {
        }
      });
  }
  loadTicketsPerYear() {
    const date = new Date().getFullYear();

    this.dashService
      .getTicketsPerYearUser(this.auth.getUserID(), date)
      .subscribe((res: APIResponse) => {
        this.ticketsOfYear = res.data[0].ticketsCount;
        console.log(this.ticketsOfYear);
      });
  }

  getTicketsGroupByStat() {
    this.dashService
      .userGetTicketsGroupByStatus(this.auth.getUserID())
      .subscribe((res: APIResponse) => {
        this.ticketsByStatus = Object(res.data);
        this.initChart();
      });
  }
  getTicketsGroupByPriority() {
    this.dashService
      .userGetTicketByPriority2(this.auth.getUserID())
      .subscribe((res: APIResponse) => {
        this.ticketsByPriority = Object(res.data);
        this.initChatTicketsByPriority();
      });
  }

  getTopEmployees() {
    this.dashService.getTopEmployeesManger().subscribe((res: APIResponse) => {
      this.topEmployees = res.data;
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
