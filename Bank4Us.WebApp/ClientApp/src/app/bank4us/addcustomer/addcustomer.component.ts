import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators} from "@angular/forms";
import { CustomerService } from "../serviceapp/proxies/customer.service";
import { Customer } from "../serviceapp/proxies/customer.model";
import { Router } from "@angular/router";

@Component({
  selector: 'app-add-customer',
  templateUrl: './addcustomer.component.html',
  providers: [CustomerService]
})
export class AddCustomerComponent implements OnInit {

  constructor(private formBuilder: FormBuilder,
    private router: Router,
    private customerService: CustomerService) { }
  
  addForm: FormGroup;

    ngOnInit() {

        //INFO: Init the form.   
        this.addForm = this.formBuilder.group({
            firstName: [],
            lastName: [],
            email: [],
            age: []
        });
    }

  onSubmit() {

    //INFO: Only submit the new customer if the form is valid.  
    if (this.addForm.valid) {

    //INFO: Map the form data into the customer object.
    let addcustomer = new Customer();

    addcustomer.firstName = this.addForm.value.firstName;
    addcustomer.lastName = this.addForm.value.lastName;
    addcustomer.emailAddress = this.addForm.value.email;
    addcustomer.age = this.addForm.value.age;
    addcustomer.accounts = [];
    addcustomer.createdBy = "admin";

    this.customerService.createCustomer(addcustomer)
    .subscribe(data => {
      this.router.navigate(['listcustomer']);
    });
    }
  }
}
