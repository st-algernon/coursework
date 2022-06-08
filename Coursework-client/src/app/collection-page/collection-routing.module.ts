import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CollectionPageComponent } from './collection-page.component';
import { UserCollectionsPageComponent } from '../user-collections-page/user-collections-page.component';
import { EditCollectionPageComponent } from './edit-collection-page/edit-collection-page.component';
import { ItemPageComponent } from './item-page/item-page.component';
import { AuthGuard } from '../shared/guards/auth.guard';

const routes: Routes = [
  { path: ":id", component: CollectionPageComponent },
  { path: ":id/edit", component: EditCollectionPageComponent, canActivate: [AuthGuard] },
  { path: ":id/item/:itemId", component: ItemPageComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CollectionRoutingModule { }
