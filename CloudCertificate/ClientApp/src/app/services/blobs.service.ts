import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { isDevMode } from "@angular/core";

@Injectable({ providedIn: 'root' })
export class BlobsService {
    finalBaseUrl?: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
        if(isDevMode()){
            this.finalBaseUrl = "https://localhost:44395";
        }
        else{
            this.finalBaseUrl = this.baseUrl;
        }
        this.finalBaseUrl += "/api";
    }

    getAllBlobs(): Observable<string[]> {
        return this.http.get<string[]>(this.finalBaseUrl + '/blobs/');
    }

    uploadFile(file: File): Observable<any>{
        const formData = new FormData();
        formData.append('file', file, file.name);
        return this.http.post(this.finalBaseUrl + '/blobs/upload-file', formData);
    }

    deleteAllBlobs(): Observable<any> {
        return this.http.delete(this.finalBaseUrl + '/blobs/delete-all');
    }
}