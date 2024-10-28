import { AfterViewChecked, Directive, ElementRef, HostListener, Input, OnDestroy, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { PermissionService } from "src/auth/services/permission.service";
import { SearchUnsubscriberService } from "src/shared/services/search-unsubscriber/search-unsubscriber.service";
import { CommunicationHubService } from "src/signalR/communication-hub.service";
import { BaseCommunicationDto } from "../../dtos/base-communication.dto";
import { NgForm } from "@angular/forms";
import { FilterDto } from "src/shared/filter-dtos/filter.dto";
import { CommunicationResource } from "../../resources/communication.resource";
import { catchError, throwError } from "rxjs";
import { HttpErrorResponse } from "@angular/common/http";
import { Configuration } from "src/app/configuration/configuration";

@Directive()
export class BaseCommunicationContainerComponent<T extends BaseCommunicationDto, TFilter extends FilterDto, TResource extends CommunicationResource<T, TFilter>> implements AfterViewChecked, OnDestroy {

    loadingData = false;
    loadingMore = false;
    sendingMessage = false;
    canLoadMore = false;
    autoScrollEnabled = false;

    currentCommunicationDto: T = this.initializeCommunication(this.structuredTType);

    entityId: number;
    @Input('entityId')
    set entityIdSetter(entityId: number) {
        this.entityId = entityId;
        this.initialization();
    }

    @ViewChild(NgForm) messageForm: NgForm;
    @ViewChild('chatHistory', { static: false }) private chatHistoryContainer: ElementRef;
    @ViewChildren('communicationMessageItem') itemElements: QueryList<any>;

    private scrollContainer: any;

    @HostListener('document:keydown.enter', ['$event']) onKeydownEnterHandler() {
        this.sendMessage();
    }

    filter: TFilter = this.initializeFilter(this.structuredFilterType);
    communications: T[] = [];

    constructor(
        protected resource: TResource,
        protected structuredTType: new () => T,
        protected structuredFilterType: new () => TFilter,
        protected permissionService: PermissionService,
        protected searchUnsubscriberService: SearchUnsubscriberService,
        protected communicationHubService: CommunicationHubService,
        protected configuration: Configuration) {
    }

    sendMessage() {
        if (this.configuration.useAllFunctionalities && this.messageForm?.valid && !this.sendingMessage) {
            this.sendingMessage = true;

            this.resource
                .sendMessage(this.currentCommunicationDto)
                .pipe(
                    catchError((err: HttpErrorResponse) => {
                        this.currentCommunicationDto.text = null;
                        this.sendingMessage = false;
                        return throwError(() => err);
                    })
                )
                .subscribe(() => {
                    this.currentCommunicationDto.text = null;
                    this.sendingMessage = false;
                });
        }
    }

    loadMore() {
        if (this.canLoadMore) {
            this.loadingData = false;
            this.loadingMore = true;
            this.getData(true, false);
        }
    }

    protected getData(loadMore: boolean, triggerLoadingDataIndicator = true) {
        this.searchUnsubscriberService.unsubscribeByType(1);
        this.loadingData = triggerLoadingDataIndicator;

        this.filter.limit = 30;

        if (loadMore) {
            this.filter.offset = this.communications.length;
        } else {
            this.filter.offset = 0;
        }

        var subscriber = this.resource
            .getCommunications(this.filter)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.loadingData = false;
                    this.loadingMore = false;
                    return throwError(() => err);
                })
            )
            .subscribe(e => {
                if (loadMore) {
                    this.autoScrollEnabled = false;
                    this.communications = [...e, ...this.communications];
                } else {
                    this.autoScrollEnabled = true;
                    this.communications = e ?? [];

                    setTimeout(() => {
                        this.scrollToBottom('auto');
                    }, 5);
                }

                this.canLoadMore = e?.length === this.filter.limit;
                this.loadingData = false;
                this.loadingMore = false;
            });

        this.searchUnsubscriberService.addSubscription(1, subscriber);
        return subscriber;
    }

    onScroll(event: any) {
        if (!this.loadingMore && !this.loadingData && this.canLoadMore && event.target.scrollTop == 0) {
            this.loadMore();
        }
    }

    private onItemElementsChanged(scrollType: string) {
        if (this.autoScrollEnabled) {
            this.scrollToBottom(scrollType);
        }
    }

    private scrollToBottom(scrollType: string) {
        this.scrollContainer?.scroll({
            top: this.scrollContainer.scrollHeight,
            left: 0,
            behavior: scrollType
        });
    }

    initializeCommunication(M: { new(): T }) {
        return new M();
    }

    initializeFilter(C: { new(): TFilter }) {
        return new C();
    }

    initialization() {

    }

    ngAfterViewChecked() {
        this.scrollContainer = this.chatHistoryContainer?.nativeElement;
        this.itemElements.changes.subscribe(_ => this.onItemElementsChanged('smooth'));
    }

    ngOnDestroy() {
        this.communicationHubService.leaveGroup();
    }
}