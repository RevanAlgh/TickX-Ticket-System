import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ClientsService } from '../../../admin/services/clients.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpBackend, HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment.development';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-chatbot',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, TranslateModule],
  templateUrl: './chatbot.component.html',
  styleUrl: './chatbot.component.css',
})
export class ChatbotComponent implements OnInit {
  @ViewChild('chatbot') chatbot!: ElementRef;
  @ViewChild('scrollMe') private myScrollContainer!: ElementRef;

  chatDesplayed = true;
  message!: string;
  responses: any[] = [];
  //messeges: string[] = [];
  messages!: { message: ''; bot: true };
  constructor(
    private clientService: ClientsService,
    private http: HttpClient,
    private handler: HttpBackend
  ) {
    this.http = new HttpClient(handler);
  }
  ngOnInit(): void {
    this.responses.push({
      message: 'Hello! How may I help you?',
      style: 'message left',
      bot: true,
    });
  }

  adjustChatbotPosition(): void {
    const chatbotElement = this.chatbot.nativeElement;

    const lang = localStorage.getItem('lang');
    if (lang === 'ar') {
      chatbotElement.style.right = 'unset';
      chatbotElement.style.left = '5%';
    } else {
      chatbotElement.style.left = 'unset';
      chatbotElement.style.right = '5%';
    }
  }
  ngAfterViewChecked() {
    this.adjustChatbotPosition();
    this.scrollToBottom();
  }

  scrollToBottom(): void {
    try {
      this.myScrollContainer.nativeElement.scrollTop =
        this.myScrollContainer.nativeElement.scrollHeight;
    } catch (err) {}
  }

  toggleChat() {
    this.chatDesplayed = !this.chatDesplayed;
  }
  send() {
    const message = this.message;
    this.message = '';
    this.responses.push({
      message: message,
      style: 'message right',
      bot: false,
    });

    this.http
      .post(
        environment.API_URL + 'ChatGPT/ask',
        { prompt: message },
        {
          responseType: 'text',
          headers: { Authorization: 'Bearer ' + localStorage.getItem('token') },
        }
      )
      .subscribe((res) => {
        console.log(res);
        this.responses.push({ message: res, style: 'message left', bot: true });
      });
  }
}
