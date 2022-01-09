import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot} from "@angular/router";
import {Observable} from "rxjs";
 
export class FilesGuard implements CanActivate{
 role:any;
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) : Observable<boolean> | boolean{
        this.role = localStorage.getItem('user');
        if (this.role !== null) {
            return true;
        } else {
            return false;
        }
    }
}