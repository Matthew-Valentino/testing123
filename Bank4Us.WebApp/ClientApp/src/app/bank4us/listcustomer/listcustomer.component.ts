import { Component, Inject } from '@angular/core';
import { Customer } from "../serviceapp/proxies/customer.model";
import { CustomerService } from "../serviceapp/proxies/customer.service";

@Component({
  selector: 'app-list-customer',
  templateUrl: './listcustomer.component.html',
  providers: [CustomerService]
})
export class ListCustomerComponent {
  public customers: Customer[];

  constructor(private customerService: CustomerService) {
    customerService.getCustomers().subscribe(result => {
      this.customers = result;
    }, error => console.error(error));
  }
}


