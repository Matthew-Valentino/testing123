import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders  } from '@angular/common/http';
import { Customer } from "./customer.model";
import { RequestOptions } from "@angular/http";

@Injectable()
export class CustomerService {
  constructor(private http: HttpClient) { }
  baseUrl: string = 'https://localhost:44346/api/Customer';

  getCustomers() {
    return this.http.get<Customer[]>(this.baseUrl + '/customers');
  }

  getCustomersWithToken(token: string) {

   let headers = new HttpHeaders();
   headers = headers.set("Authorization", "Bearer " + token);
   return this.http.get<Customer[]>(this.baseUrl + '/customers', { headers });
  }

  getCustomerById(id: number) {
    return this.http.get<Customer>(this.baseUrl + '/customers/' + id);
  }

  createCustomer(customer: Customer) {
    return this.http.post<Customer>(this.baseUrl, customer);
  }

  updateCustomer(customer: Customer) {
    return this.http.put<Customer>(this.baseUrl, customer);
  }

  deleteCustomer(id: number) {
    return this.http.delete(this.baseUrl + '/' + id);
  }
}
