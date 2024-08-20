import { Component } from '@angular/core';
import {
  Router,
  RouterLink,
  RouterModule,
  RouterOutlet,
} from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { ChatbotComponent } from '../../../client/components/chatbot/chatbot.component';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-employee-layout',
  standalone: true,
  imports: [TranslateModule, RouterLink, RouterOutlet, ChatbotComponent],
  templateUrl: './employee-layout.component.html',
  styleUrl: './employee-layout.component.css',
})
export class EmployeeLayoutComponent {
  constructor(private auth: AuthService, private router: Router) {}

  logout() {
    this.auth.logout();
    this.router.navigate(['./login']);
  }
}
