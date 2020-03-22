(ns brianium.madlib.core
  (:require [clojure.spec.alpha :as s]
            [brianium.madlib.input :as input]
            [brianium.madlib.spec :as madlib]))

;;; Madlib execution

(defn- order-answers
  [[ordered-answer]]
  (second ordered-answer))

(defn- render
  [render-fn answers]
  (render-fn
   (map second answers)
   answers))

(defn exec
  [spec render-fn]
  (->> spec
       (reduce input/ask [])
       (sort-by order-answers)
       (render render-fn)
       (println)))

(s/fdef exec
  :args (s/cat :spec ::madlib/madlib-spec :render-fn ::madlib/render-fn)
  :ret  any?)

;;; Story Building

(defn pos [p order] (fn [_] [p order]))

(s/fdef pos
  :args (s/cat :p ::madlib/part-of-speech :order int?)
  :ret  ::madlib/part-of-speech-gen)
