var myCarousel = document.querySelector('#heroBanner')
var carousel = new bootstrap.Carousel(myCarousel, {
    interval: 10000, // 10000ms = 10 giây
    wrap: true,      // Hết ảnh thì quay lại từ đầu
    pause: 'hover'   // Di chuột vào thì tạm dừng để khách xem
})