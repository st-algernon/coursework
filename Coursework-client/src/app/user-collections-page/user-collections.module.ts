import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../shared/shared.module';
import { UserCollectionsPageComponent } from './user-collections-page.component';
import { UserCollectionsRoutingModule } from './user-collections-routing.module';
import { CreateCollectionPageComponent } from './create-collection-page/create-collection-page.component';

@NgModule({
  declarations: [
      UserCollectionsPageComponent,
      CreateCollectionPageComponent
  ],
  imports: [
    CommonModule,
    UserCollectionsRoutingModule,
    SharedModule
  ],
  providers: []
})
export class UserCollectionsModule { }
