import { Component } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { ClientDashboardComponent } from '../client-dashboard/client-dashboard.component';
import {
  Router,
  RouterLink,
  RouterModule,
  RouterOutlet,
} from '@angular/router';
import { ChatbotComponent } from '../chatbot/chatbot.component';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [
    TranslateModule,
    RouterLink,
    RouterOutlet,
    ClientDashboardComponent,

    RouterModule,
  ],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.css',
})
export class LayoutComponent {
  constructor(private auth: AuthService, private router: Router) {}

  logout() {
    this.auth.logout();
    this.router.navigate(['./login']);
  }
}
