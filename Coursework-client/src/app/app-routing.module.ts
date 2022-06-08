import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './shared/guards/auth.guard';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';
import { LayoutComponent } from './shared/layout/layout.component';
import { NotAuthGuard } from './shared/guards/not-auth.guard';
import { AdminGuard } from './shared/guards/admin.guard';
import { UserActiveGuard } from './shared/guards/user-active.guard';

const routes: Routes = [
  { path: 'auth', loadChildren: () => import('./auth-page/auth.module').then(m => m.AuthModule), canActivate: [NotAuthGuard] },
  { path: '', component: LayoutComponent, children: 
    [
      { path: '', loadChildren: () => import('./home-page/home.module').then(m => m.HomeModule) },
      { path: 'admin-panel', loadChildren: () => import('./admin-panel/admin-panel.module').then(m => m.AdminPanelModule), canActivate: [AdminGuard] },
      { path: 'collection', loadChildren: () => import('./collection-page/collection.module').then(m => m.CollectionModule), canActivate: [UserActiveGuard] },
      { path: 'search', loadChildren: () => import('./search-page/search.module').then(m => m.SearchModule), canActivate: [UserActiveGuard] },
      { path: ':userId', loadChildren: () => import('./user-collections-page/user-collections.module').then(m => m.UserCollectionsModule), canActivate: [UserActiveGuard] },
      { path: '**', component: NotFoundComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
