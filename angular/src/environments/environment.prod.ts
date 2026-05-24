import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'Vosita',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44370/',
    redirectUri: baseUrl,
    clientId: 'Vosita_App',
    responseType: 'code',
    scope: 'offline_access Vosita',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44370',
      rootNamespace: 'Vosita',
    },
  },
} as Environment;
