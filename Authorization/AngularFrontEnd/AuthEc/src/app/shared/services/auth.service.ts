import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TOKEN_KEY } from '../constants';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http:HttpClient) { }
  baseUrl = 'https://localhost:7164/api';

  createUser(formData :any )
  {
    return this.http.post(this.baseUrl+ "/signup",formData )
  }

  signin(formData :any )
  {
    return this.http.post(this.baseUrl+ "/signin",formData )
  }
  isLoggedIn(){
    return localStorage.getItem(TOKEN_KEY)!=null?true:false
  }

  deleteToken(){
    localStorage.removeItem(TOKEN_KEY);
  }
  svaveToken(token: string){
    localStorage.setItem(TOKEN_KEY, token)
  }

}
  