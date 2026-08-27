import { Routes } from '@angular/router';
import { Home } from './Pages/home/home';
import { Login } from './Pages/login/login';
import { Registro } from './Pages/registro/registro';
import { guestGuard } from './Guards/guest.guard';
import { authGuard } from './Guards/auth.guard';
import { adminGuard } from './Guards/admin.guard';
import { ProductosAdmin } from './Pages/admin/productos-admin/productos-admin';
import { ProductoForm } from './Pages/admin/producto-form/producto-form';
import { CarritoPage } from './Pages/carrito/carrito';
import { Landing } from './Pages/landing/landing';
import { ProductoDetalle } from './Pages/producto-detalle/producto-detalle';
import { CategoriasAdmin } from './Pages/admin/categorias-admin/categorias-admin';
import { Catalogo } from './Pages/catalogo/catalogo';
import { Checkout } from './Pages/checkout/checkout';
import { MisCompras } from './Pages/mis-compras/mis-compras';
import { DashboardAdmin } from './Pages/admin/dashboard-admin/dashboard-admin';
import { AdminLayout } from './Pages/admin/admin-layout/admin-layout';

export const routes: Routes = [
  { path: '', component: Landing },
  { path: 'home', component: Home },
  { path: 'catalogo', component: Catalogo },
  { path: 'productos/:id', component: ProductoDetalle },
  { path: 'login', component: Login, canActivate: [guestGuard] },
  { path: 'registro', component: Registro, canActivate: [guestGuard] },
  { path: 'carrito', component: CarritoPage, canActivate: [authGuard] },
  { path: 'checkout', component: Checkout, canActivate: [authGuard] },
  { path: 'mis-compras', component: MisCompras, canActivate: [authGuard] },
  { path: 'mis-compras/:id', component: MisCompras, canActivate: [authGuard] },

  {
    path: 'admin',
    component: AdminLayout,
    canActivate: [adminGuard],
    children: [
      { path: '', redirectTo: 'productos', pathMatch: 'full' },
      { path: 'productos', component: ProductosAdmin },
      { path: 'productos/nuevo', component: ProductoForm },
      { path: 'productos/editar/:id', component: ProductoForm },
      { path: 'categorias', component: CategoriasAdmin },
      { path: 'estadisticas', component: DashboardAdmin }
    ]
  },

  { path: '**', redirectTo: 'home' }
];


