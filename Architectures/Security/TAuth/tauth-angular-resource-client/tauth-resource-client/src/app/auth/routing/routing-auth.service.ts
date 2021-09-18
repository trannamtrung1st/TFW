import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';

import { AuthModule } from '../auth.module';

import { PolicyInjector } from '../policies/policy-injector';

import { A_ROUTING } from '@app/constants';

import { AuthContext } from '../models/auth-context.model';
import { RoutingData } from '@cross/routing/models/routing-data.model';

@Injectable({
  providedIn: AuthModule
})
export class RoutingAuthService implements CanActivate {

  constructor(private _router: Router) { }

  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean | UrlTree> {
    const user = { subject: '' };
    const authContext = new AuthContext(!!user, user);
    const routeData = route.data as RoutingData | undefined;
    const policies = routeData?.policies;

    if (policies) {
      for (let policyType of policies) {
        const policy = PolicyInjector.get(policyType);
        if (!policy) throw new Error(`Policy '${policies}' not found`);
        await policy.authorizeAsync(authContext);
      }

      const authResult = authContext.authResult;

      if (authResult.isSuccess) return true;
      if (authResult.unauthorized)
        return this._router.parseUrl(routeData?.loginPath || A_ROUTING.notFound);
      if (authResult.accessDenied)
        return this._router.parseUrl(routeData?.accessDeniedPath || A_ROUTING.notFound);
    }

    return true;
  }
}
