import { SafeUrl } from "@angular/platform-browser";

export interface Image
{
    path : string,
    safeUrl: SafeUrl
    blob: Blob
}