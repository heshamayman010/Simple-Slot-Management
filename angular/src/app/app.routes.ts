import { Routes } from '@angular/router';


export const appRoutes: Routes = [
  
  
    {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full',
  },
  {
    path: 'add-slots',
    loadComponent: () => import('./components/add-slots/add-slots.component').then(m => m.AddSlotsComponent),
  },
  {
    path: 'slots-list',
    loadComponent: () => import('./components/slot-list/slot-list.component').then(m => m.SlotListComponent),
  },

  {
    path: 'home',
    pathMatch: 'full',
    loadChildren: () => import('./home/home.routes').then(m => m.homeRoutes),
  },
  
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(m => m.createRoutes()),
  },
  {
    path: 'identity',
    loadChildren: () => import('@abp/ng.identity').then(m => m.createRoutes()),
  },
  {
    path: 'tenant-management',
    loadChildren: () =>
      import('@abp/ng.tenant-management').then(m => m.createRoutes()),
  },
  {
    path: 'setting-management',
    loadChildren: () =>
      import('@abp/ng.setting-management').then(m => m.createRoutes()),
  },
];



