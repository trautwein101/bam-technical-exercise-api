import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AstronautDutySearch } from '../../components/astronaut-duty-search/astronaut-duty-search';
import { AstronautDutyCreate } from '../../components/astronaut-duty-create/astronaut-duty-create';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, AstronautDutySearch, AstronautDutyCreate],
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.css',
})
export class AdminDashboard {}
