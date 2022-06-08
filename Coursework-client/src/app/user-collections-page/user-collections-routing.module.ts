import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../shared/guards/auth.guard';
import { UserCollectionsPageComponent } from '../user-collections-page/user-collections-page.component';
import { CreateCollectionPageComponent } from './create-collection-page/create-collection-page.component';

const routes: Routes = [
  { path: '', redirectTo: 'collections' },
  { path: 'collections', component: UserCollectionsPageComponent },
  { path: 'collections/create', component: CreateCollectionPageComponent, canActivate: [AuthGuard] }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserCollectionsRoutingModule { }
