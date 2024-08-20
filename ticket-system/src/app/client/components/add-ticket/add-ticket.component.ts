import { Component, OnInit } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { TicketServiceService } from '../../../shared/services/ticket-service.service';
import { FileReaderService } from '../../../shared/services/file-reader.service';
import { SendTicketModel } from '../../../core/model/SendTicketModal';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Observable, ReplaySubject, Subscriber } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { Product } from '../../../core/model/Product';
import { ProductService } from '../../../shared/services/product.service';
import { APIResponse } from '../../../core/model/APIResponse';
import { Router, RouterLink } from '@angular/router';
import { AlertboxService } from '../../../shared/services/alertbox.service';

@Component({
  selector: 'app-add-ticket',
  standalone: true,
  imports: [
    TranslateModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    RouterLink,
  ],
  templateUrl: './add-ticket.component.html',
  styleUrl: './add-ticket.component.css',
})
export class AddTicketComponent implements OnInit {
  files: string[] = []; // Array to store Base64 strings
  fileNames: string[] = []; // Array to store file names
  errorMessage: string = ''; // Variable to hold the error message
  maxFileSize = 2 * 1024 * 1024; // 2MB in bytes
  supportedFormats: string[] = [
    'image/png',
    'image/jpeg',
    'image/jpg',
    'application/pdf',
  ];

  ticketObj = new SendTicketModel();

  products: Product[] = [];
  selectedProductId!: number;

  ticketForm = new FormGroup({
    ticketDescription: new FormControl('', Validators.required),
    title: new FormControl('', Validators.required),
    fileName: new FormControl(''),
    productId: new FormControl('', Validators.required),
  });

  constructor(
    private ticketService: TicketServiceService,
    private auth: AuthService,
    private productService: ProductService,
    private router: Router,
    private alert: AlertboxService
  ) {}
  ngOnInit(): void {
    this.loadProducts();
  }

  onFileSelected(event: any) {
    const selectedFiles = event.target.files as FileList;
    this.errorMessage = ''; // Reset the error message

    for (let i = 0; i < selectedFiles.length; i++) {
      const file = selectedFiles[i];

      if (this.supportedFormats.includes(file.type)) {
        if (file.size <= this.maxFileSize) {
          this.fileNames.push(file.name);

          const reader = new FileReader();
          reader.onload = (e: any) => {
            const base64String = e.target.result.split(',')[1];
            this.files.push(base64String);
          };
          reader.readAsDataURL(file); // Convert file to Base64
        } else {
          this.errorMessage = `${file.name} exceeds the 2MB size limit.`;
        }
      } else {
        this.errorMessage = `${file.name} is not a supported format.`;
      }
    }

    // Reset the input element to allow re-selection of the same file
    event.target.value = '';
  }

  loadProducts() {
    this.productService.getPorudcts().subscribe((res: APIResponse) => {
      this.products = res.data;
    });
  }
  onProductChange(event: Event): void {
    this.selectedProductId = parseInt(
      (event.target as HTMLSelectElement).value
    );
  }
  remove(i: number): void {
    this.fileNames.splice(i, 1);

    this.files.splice(i, 1);
  }

  send() {
    if (this.ticketForm.invalid) {
      this.alert.error('Please Enter Valid Data');
      return;
    }
    this.ticketObj = new SendTicketModel();

    this.ticketObj.createdBy = this.auth.getUserID();
    this.ticketObj.fileName = this.fileNames;
    this.ticketObj.attachment = this.files.map((file) => {
      return file;
    });

    console.log(this.fileNames);
    this.ticketObj.priority = 1;
    this.ticketObj.status = 1;
    this.ticketObj.productId = this.selectedProductId;
    this.ticketObj.title = this.ticketForm.value.title!;
    this.ticketObj.ticketDescription = this.ticketForm.value.ticketDescription!;

    this.ticketService.addTicket(this.ticketObj).subscribe(
      (res) => {
        if (res.status) {
          this.alert.success('Ticket sent successfully');
          this.router.navigate(['/client/view-tickets']);
        } else {
          this.alert.success('Error', 'Erros occurs while uploading the data');
        }
      },
      (err) => {
        this.alert.error(err.error.message);
      }
    );
    console.log(this.ticketObj);
  }
}
