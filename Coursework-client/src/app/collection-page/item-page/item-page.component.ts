import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { EditItemDialogComponent } from 'src/app/shared/components/edit-item-dialog/edit-item-dialog.component';
import { CommentsHub } from 'src/app/shared/hubs/comments.hub';
import { Item, Comment, User, CommentRequest } from 'src/app/shared/interfaces';
import { AuthStorage } from 'src/app/shared/services/auth.storage';
import { CommentsService } from 'src/app/shared/services/comments.service';
import { ItemsService } from 'src/app/shared/services/items.service';
import { UsersService } from 'src/app/shared/services/users.service';

@Component({
  selector: 'app-item-page',
  templateUrl: './item-page.component.html',
  styleUrls: ['./item-page.component.scss']
})
export class ItemPageComponent implements OnInit, OnDestroy {
  isSmallResolution?: boolean;
  dateLang: string = 'en';
  item?: Item;
  currentUser?: User;
  comments: Comment[] = [];
  commentCtrl = new FormControl();
  subs: Subscription[] = [];

  @ViewChild('likesCount') countOfLikesRef?: ElementRef<HTMLElement>;

  constructor(
    private route: ActivatedRoute,
    private itemsService: ItemsService,
    private usersService: UsersService,
    private commentsService: CommentsService,
    private dialog: MatDialog,
    private router: Router,
    private commentsHub: CommentsHub,
    private authStorage: AuthStorage,
    private translate: TranslateService,
    private responsive: BreakpointObserver
  ) { 
    translate.addLangs(['en', 'ru']);
    translate.setDefaultLang(this.dateLang);

    this.authStorage.lang$.subscribe(
      (response: string) => { 
        this.dateLang = response;
        this.translate.use(response) 
      }
    );
  }

  ngOnInit(): void {
    const itemId = this.route.snapshot.params['itemId'];

    this._observeSmallResolution();

    this.commentsHub.startConnection();
    this.commentsHub.addReceivedCommentsListener();

    this.subs.push(
      this.itemsService.getItem(itemId).subscribe(
        (response: Item) => this.item = response
      ),
      this.usersService.currentUser$.subscribe(
        (response: User) => this.currentUser = response
      ),
      this.commentsService.getComments(itemId).subscribe(
        (response: Comment[]) => this.comments = response
      ),
      this.commentsHub.comment$.subscribe(
        (response: Comment) => this.comments.unshift(response)
      )
    );
  }

  ngOnDestroy(): void {
    this.subs.forEach(
      s => s.unsubscribe()
    );
  }

  toggleLikeBtn() {
    if (this.item) {
      this.subs.push(
        this.itemsService.likeItem(this.item.id).subscribe(
          () => { 
            if (this.item) {
              if (this.item.usersItemVM.isLiked) {
                this.item.usersItemVM.isLiked = false;
                this.item.usersItemVM.countOfLikes -= 1;
              } else {
                this.item.usersItemVM.countOfLikes += 1;
                this.item.usersItemVM.isLiked = true;
              }
            }
          }
        )
      );
    }
  }

  deleteItem() {
    if (this.item?.id) {
      this.subs.push(
        this.itemsService.deleteItem(this.item.id).subscribe(
          () => this.router.navigate(['../../'],  { relativeTo: this.route })
        )
      );
    }
  }

  openEditItemDialog(item: Item) {
    this.dialog.open(EditItemDialogComponent, { 
      width: this.isSmallResolution ? '100%' : '50%', 
      data: item
    });
  }

  submitComment() {
    if (this.currentUser && this.item) {

      const request: CommentRequest = {
        text: this.commentCtrl.value,
        itemId: this.item.id,
        authorId: this.currentUser.id
      };

      this.commentsHub.sendComment(request);

      this.commentCtrl.reset();
    }
  }

  private _observeSmallResolution() {
    this.subs.push(
      this.responsive.observe([Breakpoints.XSmall, Breakpoints.Small]).subscribe(
        result => {
        if (result.matches) {
          this.isSmallResolution = true;
        } else {
          this.isSmallResolution = false;
        }
      })
    );
  }
}
