import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { EditItemDialogComponent } from 'src/app/shared/components/edit-item-dialog/edit-item-dialog.component';
import { CommentsHub } from 'src/app/shared/hubs/comments.hub';
import { CreateCommentCommand } from 'src/app/shared/interfaces';
import { CommentsClient, CommentVm, ItemsClient, ItemVm, UserVm } from 'src/app/shared/services/api.service';
import { AuthStorage } from 'src/app/shared/services/auth.storage';

@Component({
  selector: 'app-item-page',
  templateUrl: './item-page.component.html',
  styleUrls: ['./item-page.component.scss']
})
export class ItemPageComponent implements OnInit, OnDestroy {
  isSmallResolution?: boolean;
  dateLang: string = 'en';
  item?: ItemVm;
  currentUser?: UserVm;
  comments: CommentVm[] = [];
  commentCtrl = new FormControl();
  subs: Subscription[] = [];

  @ViewChild('likesCount') countOfLikesRef?: ElementRef<HTMLElement>;

  constructor(
    private route: ActivatedRoute,
    private itemsClient: ItemsClient,
    private commentsClient: CommentsClient,
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
      this.itemsClient.getItem(itemId).subscribe(
        (response: ItemVm) => this.item = response
      ),
      this.commentsClient.getComments(itemId).subscribe(
        (response: CommentVm[]) => this.comments = response
      ),
      this.authStorage.currentUser$.subscribe(
        (response: UserVm) => this.currentUser = response
      ),
      this.commentsHub.comment$.subscribe(
        (response: CommentVm) => this.comments.unshift(response)
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
        this.itemsClient.likeItem(this.item.id).subscribe(
          () => { 
            if (this.item) {
              if (this.item.usersItemVm.isLiked) {
                this.item.usersItemVm.isLiked = false;
                this.item.usersItemVm.countOfLikes -= 1;
              } else {
                this.item.usersItemVm.countOfLikes += 1;
                this.item.usersItemVm.isLiked = true;
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
        this.itemsClient.removeItem(this.item.id).subscribe(
          () => this.router.navigate(['../../'],  { relativeTo: this.route })
        )
      );
    }
  }

  openEditItemDialog(item: ItemVm) {
    this.dialog.open(EditItemDialogComponent, { 
      width: this.isSmallResolution ? '100%' : '50%', 
      data: item
    });
  }

  submitComment() {
    if (this.currentUser && this.item) {

      const request: CreateCommentCommand = {
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
