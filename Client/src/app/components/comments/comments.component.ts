import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Comments } from '../../interfaces/comments';
import { RatingComponent } from "../rating/rating.component";
import { SignalRService } from '../../services/signal-r.service';

@Component({
  selector: 'app-comments',
  standalone: true,
  imports: [CommonModule, RatingComponent],
  templateUrl: './comments.component.html',
  styleUrl: './comments.component.scss'
})
export class CommentsComponent implements OnInit, OnChanges{
  @Input() comments: Comments[] | undefined = undefined;
  @Input() showHR: boolean = true;

  constructor(
    private signalRService: SignalRService
  ){}

  ngOnInit(): void {
    
    console.log(this.comments)
    // this.signalRService.addListenerForComment((comment: Comments) => {
    //   console.log('Stigo malisa!');
    //   if(this.comments !== undefined)
    //     this.comments = [comment, ...this.comments!];
    //   else this.comments = [comment];
    // });

    ///////// ovo ne moze da bude tu. Ovo moze da bude, recimo u nav komponenti!
    // this.signalRService.message.subscribe(r => {
    //   if(r === null) return;
    //   if(this.comments !== undefined)
    //     this.comments = [r, ...this.comments];
    //   else
    //     this.comments = [r];
    // });
  
  }

  ngOnChanges(changes: SimpleChanges): void {
    if(changes['userComment']){
      console.log("user comment updated: ", changes['userComment'].currentValue);
    }
  }

  trackByFn(index: number, comment: any): any{
    return index || comment.username;
  }
}
