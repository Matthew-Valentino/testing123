import { Injectable } from '@angular/core';
import { UserManager, User } from 'oidc-client';

@Injectable()
export class AuthService {
  userManager: UserManager;
  user: User;

  constructor() {
    const config = {
      authority: 'https://localhost:44346',
      client_id: 'angular-client',
      redirect_uri: 'http://localhost:4200/listcustomer/?callback',
      response_type: 'id_token token',
      scope: 'openid profile Bank4Us.ServiceApp',
      post_logout_redirect_uri: 'http://localhost:4200/'
    };
    this.userManager = new UserManager(config);
  }

  login() {
    this.userManager.signinRedirect();
  }

  logout() {
    this.userManager.signoutRedirect();
  }

  getAccessToken() {
    return this.user ? this.user.access_token : null;
  }

  initializeUser() {
    if (document.URL.indexOf('?callback') >= 0) {
      this.userManager.signinRedirectCallback().then(user => {
        this.user = user;
        window.location.href = '/listcustomer';
      });
    } else {
      this.userManager.getUser().then(user => this.user = user);
    }

  }
}
