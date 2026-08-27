import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '../../../Services/auth.service';

@Component({
  selector: 'app-admin-layout',
  imports: [RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './admin-layout.html',
  styleUrl: './admin-layout.css'
})
export class AdminLayout {
  authService = inject(AuthService);

  navegacion = [
    { path: '/admin/productos', label: 'Productos', icono: 'box' },
    { path: '/admin/categorias', label: 'Categorías', icono: 'tag' },
    { path: '/admin/estadisticas', label: 'Estadísticas', icono: 'chart' }
  ];
}
