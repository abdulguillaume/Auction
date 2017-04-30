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
        public message = 'Auctions!';

        constructor(private AuctionService: AuctionApp.Services.AuctionService,
            private $uibModal: angular.ui.bootstrap.IModalService) {

            this.auctions = AuctionService.listAuctions();
                
        }

        public refresh() {
            this.auctions = this.AuctionService.listAuctions();
        }

        public showModal(id: number, item: string, username: string) {//, $uibModal: angular.ui.bootstrap.IModalService) {
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
                    this.refresh();
                });

        }

    }


    export class AboutController {
        public message = 'Hello from the about page!';
    }

    
    class DialogController {

        public bid;

        public error = false;

        public ok() {

            let auction = {
                id: this.id,
                bids: [{ id: 0, bidAmount: this.bid, customer: this.username }]
            };

            let resource:any = this.$resource('/api/auctions/:id', null, {
                save: {
                    method: 'POST'
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
                    err => this.error = true
                );

                
            }


        constructor(private id: number, private item: string, private username: string, private $uibModalInstance: angular.ui.bootstrap.IModalServiceInstance, private $resource: ng.resource.IResourceService ){}
                //,private $state: ng.ui.IStateService) {}
    }

    angular.module('AuctionApp').controller('DialogController', DialogController);


}
