import { Component, ElementRef, Inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ItemFormComponent } from '../item-form/item-form.component';
import { ItemsClient, ItemVm, ShortItemVm } from '../../services/api.service';

@Component({
  selector: 'app-edit-item-dialog',
  templateUrl: './edit-item-dialog.component.html',
  styleUrls: ['./edit-item-dialog.component.scss']
})
export class EditItemDialogComponent implements OnInit, OnDestroy {
  seed?: ShortItemVm;
  subs: Subscription[] = [];
  isLoading: boolean = false;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: ItemVm,
    private dialogRef: MatDialogRef<EditItemDialogComponent>,
    private itemsClient: ItemsClient,
    private snackBar: MatSnackBar,
    private router: Router
  ) { }

  ngOnInit(): void {

  }

  ngOnDestroy(): void {
    this.subs.forEach(
      u => u.unsubscribe()
    );
  }

  onSubmit(item: ShortItemVm) {
    this.isLoading = true;

    this.subs.push(
      this.itemsClient.editItem(item).subscribe(
        () => {
          this.isLoading = false;
          this.snackBar.open('The item was edited successfully', '', { duration: 3000 });
          this.dialogRef.close();
          this.reloadCurrentRoute();
        },
        () => this.isLoading = false
      )
    );
  }

  onLoading(result: boolean) {
    this.isLoading = result;
  }

  private reloadCurrentRoute() {
    let currentUrl = this.router.url;
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.router.onSameUrlNavigation = 'reload';
    this.router.navigate([currentUrl]);
  }
}