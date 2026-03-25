import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AstronautDutySearch } from '../../components/astronaut-duty-search/astronaut-duty-search';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, AstronautDutySearch],
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.css',
})
export class AdminDashboard {}
