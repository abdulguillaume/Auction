namespace AuctionApp.Controllers {

    export class HomeController {
        public message = 'Hello from the home page!';
    }

    export class SecretController {
        public secrets;

        constructor($http: ng.IHttpService) {
            $http.get('/api/secrets').then((results) => {
                this.secrets = results.data;
            });
        }
    }


    export class  AuctionsController {
        public auctions; 
        public auctionToCreate;
        //to remove once test succeed
        //public files;

        constructor(private AuctionService: AuctionApp.Services.AuctionService, private $state: ng.ui.IStateService, private $uibModal: angular.ui.bootstrap.IModalService)
        {
            this.auctions = AuctionService.listAuctions();     
        }

        //to refresh the auctions list
        public refresh(that) {
            that.auctions = that.AuctionService.listAuctions();
        }

        public showModal(id: number, item: string, username: string) {

            let that = this;

            this.$uibModal.open({
                templateUrl: '/ngApp/views/auctionDialog.html',
                controller: 'DialogController',
                controllerAs: 'modal',
                resolve: {
                    id: () => id,
                    item: () => item,
                    username: () => username
                },
                size: 'sm'
            })
                .result
                .then(function () {
                    that.refresh(that);
                });

        }


        public addAuction() {
            this.AuctionService.save(this.auctionToCreate)
                .then(
                () => {
                    this.auctionToCreate = null;
                    this.$state.go('home');
                }
                );
        }





    }

    export class AddController {
        public message = 'Hello from the about page!';
    }


    export class AboutController {
        public message = 'Hello from the about page!';
    }

    
    class DialogController {

        public bid;

        public error = false;

        public emsg;

        public ok() {

            let auction = {
                id: this.id,
                bids: [{ id: 0, bidAmount: this.bid, customer: this.username }]
            };

            let resource:any = this.$resource('/api/auctions/:id', null, {
                save: {
                    method: 'POST',
                }
            });

            resource.save(auction)
                .$promise
                .then(
                    response =>
                    {
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


        constructor(private id: number, private item: string, private username: string, private $uibModalInstance: angular.ui.bootstrap.IModalServiceInstance, private $resource: ng.resource.IResourceService ){}
    }

    angular.module('AuctionApp').controller('DialogController', DialogController);


}
