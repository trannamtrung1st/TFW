import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ROUTING, ROUTING_DATA } from './constants';

import { RoutingData } from '@cross/routing/models/routing-data.model';

import { AuthenticatedUserPolicy } from '@auth/policies/authenticated-user.policy';

import { HomePageComponent } from './home-page/home-page.component';

import { RoutingAuthService } from '@auth/routing/routing-auth.service';

const routes: Routes = [
  {
    path: ROUTING.home,
    component: HomePageComponent,
    data: {
      ...ROUTING_DATA.common,
      policies: [AuthenticatedUserPolicy]
    } as RoutingData,
    canActivate: [RoutingAuthService]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
