import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ CommonModule, ReactiveFormsModule, RouterLink,  ],
  templateUrl: './login.component.html',
  styles: ``
})
export class LoginComponent {

  IsSubmitted: boolean = false;
  form : FormGroup;

  constructor(public formBuilder: FormBuilder, private service: AuthService,private router : Router, private toastr: ToastrService )
  {

    this.form = this.formBuilder.group({

      email: ['', [Validators.required, ]  ],
      password: ['', Validators.required ],

    })
  }

onSubmit() {
  this.IsSubmitted = true;

  if (this.form.valid) {
    this.service.signin(this.form.value).subscribe({
      next:(res:any)=>{
        localStorage.setItem('token', res.token);
        this.router.navigateByUrl('/dashboard');

      },
      error:err=>{

        if(err.status == 400)
            this.toastr.error('Incorrect email or password. ' , 'Login failed')
          else
            console.log('error during login: \n' , err)


      }
    })
  }

  console.log(this.form.value)
  {

  }
}
}
