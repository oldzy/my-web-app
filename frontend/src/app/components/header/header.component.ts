import { Component } from '@angular/core';
import { AuthStateService } from '../../services/auth-state.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-header',
  imports: [CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  constructor(private authStateService: AuthStateService) {}

  logout(): void {
    this.authStateService.logout();
  }
}
