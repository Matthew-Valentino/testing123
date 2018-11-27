import { Component, Inject } from '@angular/core';
import { Customer } from "../serviceapp/proxies/customer.model";
import { CustomerService } from "../serviceapp/proxies/customer.service";
import { AuthHttpService } from "../sts/auth-http.service";
import { AuthService } from "../sts/auth.service";
import { Headers, Http, RequestOptions } from '@angular/http';

@Component({
  selector: 'app-list-customer',
  templateUrl: './listcustomer.component.html',
  providers: [CustomerService, AuthHttpService, AuthService]
})
export class ListCustomerComponent {
  public customers: Customer[];

  constructor(private customerService: CustomerService, private _authService: AuthService) {  
    this._authService.initializeUser(); 
   }

   accessToken: string;

  ngOnInit() {   
    this.accessToken = this._authService.getAccessToken();
    
    this.customerService.getCustomersWithToken(this.accessToken).subscribe(result => {
      this.customers = result;
    }, error => console.error(error));
  }
  
  login() {
    this._authService.login();
}

  logout() {
    this._authService.logout();
  }
}


