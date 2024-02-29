import { Component, OnInit } from "@angular/core";
import { BlobsService } from "../services/blobs.service";
import { ToastrService } from "ngx-toastr";

@Component({
    selector:'files-component',
    templateUrl: './files.component.html',
    styleUrls: ['./files.component.scss']
})
export class FilesComponent implements OnInit {
    progress: number;
    message: string;
    blobsNames: string[];

    constructor (private blobsService: BlobsService, private toastr: ToastrService) {
    }

    ngOnInit(): void {
        this.getAllBlobs();
    }

    uploadFile = (files: FileList | null) => {
        if (files == null || files.length === 0) {
          return;
        }
        let fileToUpload = <File>files[0];
        console.log(files);

        this.blobsService.uploadFile(fileToUpload).subscribe({
            next: (res) => {
                this.toastr.success('Successfully uploaded file');
                this.getAllBlobs();
            },
            error: (res) => {
                this.toastr.error('Unable to upload file');
            }
        });
    }

    deleteAllBlobs() {
        this.blobsService.deleteAllBlobs().subscribe({
            next: (res) => {
                this.toastr.success('Successfully deleted all blobs');
                this.getAllBlobs();
            },
            error: (res) => {
                this.toastr.error('Unable to delete all blobs');
            }
        });
    }

    getAllBlobs() {
        this.blobsService.getAllBlobs().subscribe({
            next: (res) => {
                this.blobsNames = res;
            },
            error: (res) => {
                this.toastr.error('Unable to load blobs');
            }
        });
    }
}