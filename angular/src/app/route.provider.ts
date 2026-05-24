import { RoutesService, eLayoutType } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      {
        path: '/home',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      {
        path: '/add-slots',              
        name: '::Generate Slots',         
        iconClass: 'fas fa-calendar-plus', 
        order: 2,
        layout: eLayoutType.application,
      },
      {
        path: '/slots-list',
        name: '::Slots List',
        iconClass: 'fas fa-list',
        order: 3,
        layout: eLayoutType.application,
      }
    ]);
  };
}