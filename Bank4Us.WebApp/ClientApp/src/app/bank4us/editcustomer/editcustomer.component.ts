import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators} from "@angular/forms";
import { CustomerService } from "../serviceapp/proxies/customer.service";
import { Customer } from "../serviceapp/proxies/customer.model";
import { Router, ActivatedRoute, Params } from "@angular/router";

@Component({
    selector: 'app-editcustomer',
    templateUrl: './editcustomer.component.html',
    providers: [CustomerService]
})
/** editcustomer component*/
export class EditCustomerComponent implements OnInit {
    editForm: FormGroup;
    editCustomer: Customer;
    constructor(private formBuilder: FormBuilder,
        private router: Router,
        private customerService: CustomerService,
        private activatedRoute: ActivatedRoute
     ) { }

ngOnInit(){

    var customerId;

    this.activatedRoute.queryParams.subscribe(params => {
        customerId = params['customerId'];
     });

     this.customerService.getCustomerById(customerId)
     .subscribe(customer => {

        this.editCustomer = customer;

        this.editForm = this.formBuilder.group({
            customerId: customer.id,
            firstName: customer.firstName,
            lastName: customer.lastName,
            email: customer.emailAddress,
            age: customer.age
          });
     });
}

onSubmit() {

    //INFO: Only submit the new customer if the form is valid.  
    if (this.editForm.valid) {

    //INFO: Map the form data into the customer object.
    let updatedcustomer = new Customer();

    updatedcustomer.id = this.editForm.value.customerId;
    updatedcustomer.firstName = this.editForm.value.firstName;
    updatedcustomer.lastName = this.editForm.value.lastName;
    updatedcustomer.emailAddress = this.editForm.value.email;
    updatedcustomer.age = this.editForm.value.age;
    updatedcustomer.updatedBy = "admin";

    this.customerService.updateCustomer(updatedcustomer)
     .subscribe(data => {
      this.router.navigate(['listcustomer']);
     });
    }
  }
    
}
