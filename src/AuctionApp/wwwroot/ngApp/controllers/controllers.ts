namespace AuctionApp.Controllers {

    //function SetPicToNullToAvoidCircularRef(auction: AuctionJSON)
    //{
    //    if (auction.images) {
    //        //for (let i = 0; i < auction.images.length; i++) {
    //        //    auction.images[i].pic = null;
    //        //}
    //        auction.images = null;
    //    }
    //}


    class BaseAuctionJSON {
        public id: number;
        public name;
        public description;
        public minimumBid: number;
        public numberOfBids: number;
        public createdBy;
        public deletedImgs: number[] = [];//used when doing update and removing image
    }

    class AuctionJSON extends BaseAuctionJSON {
        public bids: BidJSON[];
        public images: ImgJSON[];
    }


    class ImgJSON {
        public id: number;
        public pic: object;
        //public toBeDeleted: boolean = false;
    }

    class BidJSON {
        public id: number;
        public bidAmount: number;
        public customer;
        public bidDate;
    }

    //export class HomeController {
    //    public message = 'Hello from the home page!';
    //}

    //export class SecretController {
    //    public secrets;

    //    constructor($http: ng.IHttpService) {
    //        $http.get('/api/secrets').then((results) => {
    //            this.secrets = results.data;
    //        });
    //    }
    //}

    
    

    export class AuctionsController {

        public loading;

        public username;

        public auctions: AuctionJSON[]; 

        public auctionToCreate: AuctionJSON;
        public auctionToEdit: AuctionJSON; //use for both edit and delete operation
        public auction_tmp: BaseAuctionJSON = new BaseAuctionJSON();

        public files;

        public emsg;//error message to show on the screen

        constructor(private AuctionService: AuctionApp.Services.AuctionService, private $state: ng.ui.IStateService, private $uibModal: angular.ui.bootstrap.IModalService, private Upload: angular.angularFileUpload.IUploadService, private $stateParams: ng.ui.IStateParamsService, private accountService: AuctionApp.Services.AccountService)
        {
            //debugger;
            this.auctions = this.AuctionService.listAuctions();

            if (this.$stateParams) {
                //this.auctions.
                this.auctionToEdit = this.AuctionService.getAuction(this.$stateParams.id);
            } 

        }

        public cachePropsInBaseObject(source: AuctionJSON, that) {
            let tmp;

            if (source.id)
                tmp = source.id;
            else
                tmp = 0;

            that.auction_tmp.id = tmp;
            that.auction_tmp.name = source.name;
            that.auction_tmp.description = source.description;
            that.auction_tmp.createdBy = that.username;
            that.auction_tmp.minimumBid = source.minimumBid;
            that.auction_tmp.numberOfBids = source.numberOfBids;
        }

        //OK
        //public getData() {

        //    this.auctions = this.AuctionService.listAuctions();
        //}

        public deleteAuction(id)
        {
            this.AuctionService.deleteAuction({ id: id })
                .then(response => {
                    console.log('Item deleted successfully!');
                    this.emsg = '';
                    this.$state.go('home');
                })
                .catch(err => {
                    console.log("Item could not be deleted!");
                    this.emsg = err.data;
                })
        }

        public modelStateOk(auction)
        {
            if (auction && auction.name &&
                auction.description &&
                auction.minimumBid &&
                auction.numberOfBids )
                return true;
            else
                return false;
        }

        public showModal(auctionId:number, auctionName: string, username:string) {//id: number, item: string, username: string) {

            let that = this;
            

            this.$uibModal.open({
                templateUrl: '/ngApp/views/auctionDialog.html',
                controller: 'DialogController',
                controllerAs: 'modal',
                resolve: {
                    auctionId: () => auctionId,
                    auctionName: () => auctionName,
                    username: () => username
                },
                size: 'sm'
            })
                .result
                .then(function () {
                    //that.getData();
                    that.auctions = that.AuctionService.listAuctions();
                });

        }

        public getFilesCount()
        {
            if (this.files == null) {
                return 0;
            }
            else {
                this.emsg = '';
                return this.files.length;
            }
        }

        public loadInBrowser(files)
        {
            this.files = files;
        }


        public removeImg(id) {

            this.auctionToEdit.images = this.auctionToEdit.images.filter(x => x.id != id);
            this.auction_tmp.deletedImgs.push(id);

            console.log("remove img id: " + id);
        }

        public uploadFormData(auction)
        {
          /*http://public.jsweet.org/apidocs/snapshots/org/jsweet/candies/ng-file-upload/11.1.1-SNAPSHOT/def/ng_file_upload/ng/angularfileupload/IFileUploadConfigFile.html
            */

            this.username = this.accountService.getUserName();

            this.cachePropsInBaseObject(auction, this);

            this.Upload.upload({
                method: 'post',
                data: {
                    //transformRequest: fd,
                    files: this.files,
                    fields: this.auction_tmp, 
                    headers: { 'Content-Type': undefined, enctype: 'multipart/form-data' }
                },
                url: '/api/upload'
            })
                .then((result) => {
                    console.log('File upload complete.');
                    this.emsg = '';
                    this.$state.go('home');
                }).catch((err) => {
                    console.error('Could not upload file!');
                    this.emsg = err.data;
                    this.files = null;
                });

        }

    }

    //export class AddController {
    //    public message = 'Hello from the about page!';
    //}


    //export class AboutController {
    //    public message = 'Hello from the about page!';
    //}

    
    class DialogController {

        public bid;

        public error = false;

        public emsg;

        public ok() {
           
            let tmp_auction = {
                id: this.auctionId,
                bids: [{ id: 0, bidAmount: this.bid, customer: this.username }]
            };

            let resource: any = this.$resource('/api/auctions/:id', null, {
                save: {
                    method: 'POST'
                },
                get: {
                    method: 'GET'
                }
            });

            resource.save(tmp_auction)
                .$promise
                .then(
                    response =>
                    {
                        //resource.get(this.auctionId);
                        this.$uibModalInstance.close();
                        this.error = false; 
                    }
                )
                .catch(
                    err =>
                    {
                        this.error = true;
                        this.emsg = err.data;
                    }
                );

            }


        constructor(private auctionId: number, private auctionName: string, private username: string, private $uibModalInstance: angular.ui.bootstrap.IModalServiceInstance, private $resource: ng.resource.IResourceService) {

        }
    }

    angular.module('AuctionApp').controller('DialogController', DialogController);

    angular.module('AuctionApp').config(['$qProvider', function ($qProvider) {
        $qProvider.errorOnUnhandledRejections(false);
    }]);


 
}
