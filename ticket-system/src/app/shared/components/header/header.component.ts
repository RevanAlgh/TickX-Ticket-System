import { Component, OnInit } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [TranslateModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent implements OnInit {
  currentLang: any;
  constructor(private translate: TranslateService) {}
  ngOnInit(): void {
    this.currentLang = this.translate.currentLang;
  }

  onChange() {
    if (this.currentLang == 'en') {
      this.translate.use('ar');
      localStorage.setItem('lang', 'ar');
      this.currentLang = this.translate.currentLang;
      document.documentElement.dir = 'rtl';
    } else {
      this.translate.use('en');
      localStorage.setItem('lang', 'en');
      this.currentLang = this.translate.currentLang;
      document.documentElement.dir = 'lfr';
    }
  }
}
