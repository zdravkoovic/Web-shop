import { Component, Input, OnDestroy, OnInit, Output, EventEmitter } from '@angular/core';
import { RatingService } from '../../services/rating.service';
import { RatingComponent } from '../rating/rating.component';
import { Subscription } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { Comments } from '../../interfaces/comments';
import { CommonModule } from '@angular/common';
import { CommentsComponent } from "../comments/comments.component";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SignalRService } from '../../services/signal-r.service';
import { CommentRequest } from '../../interfaces/http/comment-request';

@Component({
  selector: 'app-rate',
  standalone: true,
  imports: [ReactiveFormsModule, RatingComponent, MatButtonModule, CommonModule, CommentsComponent],
  templateUrl: './rate.component.html',
  styleUrl: './rate.component.scss'
})
export class RateComponent implements OnInit, OnDestroy {
  @Input() productId!: number | null;
  @Input() userComment: Comments | undefined | null = undefined;

  @Output() navigateToTab = new EventEmitter<string>();

  form! : FormGroup;
  
  readonly: boolean = true;
  selectedRating: number = 0;
  private rating?: Subscription;
  editComment: boolean = false;

  constructor(
    private ratingService: RatingService,
    private signalRService: SignalRService
  ){}
  
  ngOnInit(): void {

    console.log(this.userComment);

    if(this.productId == null) return;

    this.signalRService.mess();

    this.form = new FormGroup({
      comment: new FormControl('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(250)
      ])
    });

    this.rating = this.ratingService.rating$.subscribe(rate => this.selectedRating = rate);

    if (this.userComment) {
      this.form.patchValue({
        comment: this.userComment.text
      });
      this.selectedRating = this.userComment.rate;
      this.readonly = true;
    } else {
      this.readonly = false;
    }
  }

  rate(){
    if(!this.form.valid) return;
    if(this.productId === null) return;
    
    if(this.selectedRating == 0){
      alert("You have to rate first if you want to comment the product.");
      return;
    }

    // this.navigateToTab.emit('comments');
    
    var text = this.form.get("comment")?.value;
    
    var comment : CommentRequest = {
      productId: this.productId,
      rate: this.selectedRating,
      comment: text
    }
   
    this.ratingService.leaveYourComment(comment).subscribe(res => {
      this.signalRService.sendTheComment(res);
    })

    this.signalRService.message.subscribe(r => this.userComment = r);
    
    this.editComment = false;
  }

  edit(){
    this.editComment = true;
    this.readonly = false;
  }

  cancel(){
    this.editComment = false;
    this.readonly = true;
  }

  ngOnDestroy(): void {

    if(this.rating){
      this.rating.unsubscribe();
    }
  }

  goToComments(){
    // this.navigateToTab.emit();
  }
}
