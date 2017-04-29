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

        constructor(private AuctionWithBidsService: AuctionApp.Services.AuctionWithBidsService,
            private $uibModal: angular.ui.bootstrap.IModalService) {

            this.auctions = AuctionWithBidsService.listAuctions();
                
        }

        public showModal(auction:object, user:string){//, $uibModal: angular.ui.bootstrap.IModalService) {
            this.$uibModal.open({
                templateUrl: '/ngApp/views/auctionDialog.html',
                controller: 'DialogController',
                controllerAs: 'modal',
                resolve: {
                    auction: () => auction,
                    username: () => user
                },
                size: 'sm'
            });
        }

    }


    export class AboutController {
        public message = 'Hello from the about page!';
    }

    
        class DialogController {
            public bid;
           // public noError = true;

            public ok() {

                console.log(this.bid);
                /*var auction = this.AuctionWithBidsService
                    .getAuction(this.auction_id)
                    .$promise.then(
                    result => //console.log(result.item)

                    {
                        var json = JSON.stringify({
                            item: result.item,
                            bidAmount: this.bid,
                            customer: this.username
                        });

                        this.saveBid(
                            encodeURIComponent(json)
                        );

                    }
                        
                        
                        )
                    */



                /*this.saveBid(
                    {
                        item: this.auction_id,
                        bidAmount: this.bid,
                        customer: this.username
                    });*/
                

                this.$uibModalInstance.close();
            }

            public saveBid() {

                this.AuctionWithBidsService.save(this.bid);
                    /*.then(
                        () => this.$state.go("home")
                    );*/
            }

            constructor(public auction: object, public username: string, private $uibModalInstance: angular.ui.bootstrap.IModalServiceInstance, private AuctionWithBidsService: AuctionApp.Services.AuctionWithBidsService,
                private $state: ng.ui.IStateService) {
            }
    }

    angular.module('AuctionApp').controller('DialogController', DialogController);


}
