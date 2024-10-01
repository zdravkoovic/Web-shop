import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { HomeComponent } from './components/home/home.component';
import { AdminComponent } from './components/admin/admin.component';
import { authGuard } from './services/auth/auth.guard';
import { ProductDetailsComponent } from './components/product-details/product-details.component';
import { Page404Component } from './components/page-404/page-404.component';

export const routes: Routes = [
    { path: '', redirectTo: "/home", pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'signup', component: RegisterComponent },
    { path: 'home', component: HomeComponent },
    { path: 'home/product/:id', component: ProductDetailsComponent },
    { path: 'admin', component: AdminComponent, canActivate: [authGuard] },
    { path: '**', component: Page404Component}
];
