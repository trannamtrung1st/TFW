import { Component } from '@angular/core';

import { A_ROUTING } from './constants';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  A_ROUTING = A_ROUTING;

  isAuthenticated: boolean;
  isAdmin: boolean;

  constructor() {
    this.isAuthenticated = false;
    this.isAdmin = false;
  }

  onLogoutClicked() {
  }

}
