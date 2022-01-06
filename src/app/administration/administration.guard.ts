import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot} from "@angular/router";
import {Observable} from "rxjs";
 
export class AdministrationGuard implements CanActivate{
 role:any;
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) : Observable<boolean> | boolean{
        this.role = localStorage.getItem('userRole');
        console.log(this.role);
        if (this.role === 'Admin') {
            return true;
        } else {
            return false;
        }
    }
}