import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminPanelRoutingModule } from './admin-panel-routing.module';
import { UsersPageComponent } from './users-page/users-page.component';
import { SharedModule } from '../shared/shared.module';
import { UsersService } from '../shared/services/users.service';

@NgModule({
  declarations: [
    UsersPageComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    AdminPanelRoutingModule
  ],
  providers: [
    UsersService
  ]
})
export class AdminPanelModule { }
