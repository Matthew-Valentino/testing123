import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders  } from '@angular/common/http';
import { Customer } from "./customer.model";

@Injectable()
export class CustomerService {
  constructor(private http: HttpClient) { }
  baseUrl: string = 'https://localhost:44346/api/Customer';

  getCustomers() {
    return this.http.get<Customer[]>(this.baseUrl + '/customers');
  }

  getCustomerById(id: number) {
    return this.http.get<Customer>(this.baseUrl + '/customers/' + id);
  }

  createCustomer(customer: Customer) {
    return this.http.post(this.baseUrl, customer);
  }

  updateCustomer(customer: Customer) {
    return this.http.put(this.baseUrl, customer);
  }

  deleteCustomer(id: number) {
    return this.http.delete(this.baseUrl + '/' + id);
  }
}
