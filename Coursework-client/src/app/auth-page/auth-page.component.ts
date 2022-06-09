import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { AuthClient, LoginQuery, RegisterQuery } from '../shared/services/api.service';
import { AuthStorage } from '../shared/services/auth.storage';

@Component({
  selector: 'app-auth-page',
  templateUrl: './auth-page.component.html',
  styleUrls: ['./auth-page.component.scss']
})
export class AuthPageComponent implements OnInit, OnDestroy {

  submitted: boolean = false;
  loginForm: FormGroup;
  registerForm: FormGroup;

  subs: Subscription[] = [];

  constructor(
    private authClient: AuthClient,
    private router: Router,
    private snackBar: MatSnackBar,
    private route: ActivatedRoute,
    private translate: TranslateService,
    private authStorage: AuthStorage
  ) { 
    translate.addLangs(['en', 'ru']);
    translate.setDefaultLang('en');
    
    this.authStorage.lang$.subscribe(
      (response: string) => this.translate.use(response)
    );
    
    this.loginForm = new FormGroup ({
      email: new FormControl(null, [Validators.required, Validators.email]),
      password: new FormControl(null, [Validators.minLength(6), Validators.required])
    });

    this.registerForm = new FormGroup ({
      name: new FormControl(null, [Validators.required]),
      email: new FormControl(null, [Validators.required, Validators.email]),
      password: new FormControl(null, [Validators.minLength(6), Validators.required])
    });
  }

  ngOnInit(): void {
    if (this.route.snapshot.queryParams['user-blocked']) {
      this.snackBar.open('User is blocked', 'Ok');
    }

    if (this.route.snapshot.queryParams['login-again']) {
      this.snackBar.open('Please, login again', '', { duration: 3000 });
    }
  }

  ngOnDestroy(): void {
    this.subs.forEach(
      s => s.unsubscribe()
    );
  }

  login() {
    if (this.loginForm.invalid) {
      return
    }

    this.submitted = true;

    const request: LoginQuery = {
      email: this.loginForm.value.email,
      password: this.loginForm.value.password
    }

    this.subs.push(
      this.authClient.login(request).subscribe(
        () => {
          this.router.navigate(['']);
          this.submitted = false;
        }, 
        (errorResponse: HttpErrorResponse) => {
          this.snackBar.open(errorResponse.error, '', { duration: 3000 });
          this.submitted = false;
        }
      )
    );
  }

  register() {
    if (this.registerForm.invalid) {
      return
    }

    this.submitted = true;

    const request: RegisterQuery = {
      name: this.registerForm.value.name,
      email: this.registerForm.value.email,
      password: this.registerForm.value.password
    }

    this.subs.push(
      this.authClient.register(request).subscribe(
        () => {
          this.router.navigate(['']);
          this.submitted = false;
        },
        (errorResponse: HttpErrorResponse) => {
          this.snackBar.open(errorResponse.error, '', { duration: 3000 });
          this.submitted = false;
        }
      )
    );
  }
}
