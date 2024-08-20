import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { FooterComponent } from './shared/components/footer/footer.component';
import { HeaderComponent } from './shared/components/header/header.component';
import { CommonModule } from '@angular/common';
import { ChatbotComponent } from './client/components/chatbot/chatbot.component';
import { LoaderComponent } from './shared/components/loader/loader.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    TranslateModule,
    FooterComponent,
    HeaderComponent,
    CommonModule,
    ChatbotComponent,
    LoaderComponent,
  ],

  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  constructor(private translateService: TranslateService) {}
  ngOnInit(): void {
    const lang = localStorage.getItem('lang');
    if (lang) {
      lang == 'ar'
        ? (document.documentElement.dir = 'rtl')
        : (document.documentElement.dir = 'lfr');
      this.translateService.use(lang);
    } else this.translateService.use('en');
  }
}
