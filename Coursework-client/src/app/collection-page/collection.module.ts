import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CollectionRoutingModule } from './collection-routing.module';
import { CollectionPageComponent } from './collection-page.component';
import { SharedModule } from '../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { EditCollectionPageComponent } from './edit-collection-page/edit-collection-page.component';
import { ItemPageComponent } from './item-page/item-page.component';


@NgModule({
  declarations: [
    CollectionPageComponent,
    EditCollectionPageComponent,
    ItemPageComponent
  ],
  imports: [
    CommonModule,
    CollectionRoutingModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [ ]
})
export class CollectionModule { }
