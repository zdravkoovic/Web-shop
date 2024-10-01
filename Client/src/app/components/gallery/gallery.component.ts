import { Component, Input, OnInit } from '@angular/core';
import { LightgalleryModule } from 'lightgallery/angular'
import { BeforeSlideDetail } from 'lightgallery/lg-events';
import lgZoom from 'lightgallery/plugins/zoom';
import { ImageService } from '../../services/image.service';
import { Image } from '../../interfaces/image';
import { CommonModule } from '@angular/common';
import { NgbCarouselModule } from "@ng-bootstrap/ng-bootstrap";
import GLightBox from "glightbox";

@Component({
  selector: 'app-gallery',
  standalone: true,
  imports: [NgbCarouselModule ,CommonModule],
  templateUrl: './gallery.component.html',
  styleUrl: './gallery.component.scss'
})
export class GalleryComponent implements OnInit{
  
  @Input() images? : Image[];
  navigationsArrows: boolean = true;
  navigationsIndicators: boolean = true;

  settings = {
    counter: true,
    
  };

  constructor(
    private imgService: ImageService
  ){}
  
  ngOnInit(): void {
    
  }
  
  onBeforeSlide = (detail: BeforeSlideDetail): void => {
      const { index, prevIndex } = detail;
      console.log(index, prevIndex);
  };

}
