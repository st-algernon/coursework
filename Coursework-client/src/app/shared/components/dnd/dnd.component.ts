import { Component, EventEmitter, HostListener, Input, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Media } from '../../interfaces';

@Component({
  selector: 'app-dnd',
  templateUrl: './dnd.component.html',
  styleUrls: ['./dnd.component.scss']
})
export class DndComponent implements OnInit {
  fileOver: boolean = false;
  files: File[] = [];
  previewImages: Media[] = [];

  @Input() multiple = false;
  @Input() accept = '.png, .jpg, .jpeg';
  @Input() formControl = new FormControl();
  @Output() valueChanged = new EventEmitter<File[]>();

  constructor() { }

  ngOnInit(): void {
  }

  @HostListener('dragover', ['$event']) onDragOver(event: DragEvent) {
    this.fileOver = true;
  }

  @HostListener('dragleave', ['$event']) public onDragLeave(event: DragEvent) {
    this.fileOver = false;
  }

  @HostListener('drop', ['$event']) public onDrop(event: DragEvent) {
    this.fileOver = false;
  }

  ngOnChanges(): void {
    console.log(this.formControl);
  }

  deleteFile(index: number) {
    this.files.splice(index, 1);
    this.previewImages.splice(index, 1);
    this.valueChanged.emit(this.files);
  }

  resetFiles() {
    this.files = [];
    this.previewImages = [];
    this.valueChanged.emit(this.files);
  }

  previewFiles(files: File[]) {
    for (var file of files) {
      if (file.type.match('image*')) {
        const reader = new FileReader();
  
        reader.onload = (e: ProgressEvent) => {
          let result = (e.target as FileReader).result ?? '';

          this.previewImages.push({
              name: file.name,
              mimeType: file.type,
              path: result.toString(),
              size: file.size
          });
        };
  
        reader.readAsDataURL(file); 
      }
    }
  }

  onFileChange(fileList: FileList | null) {
    if (fileList) {
      this.files = Array.from(fileList);
      this.valueChanged.emit(this.files);
      this.previewFiles(this.files);
    }

    console.log(this.formControl);
  }
}